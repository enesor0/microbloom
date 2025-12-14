export const auth = {
    login: async (url, data, returnUrl) => {
        try {
            const response = await fetch(`${url}?returnUrl=${encodeURIComponent(returnUrl || '/')}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                const result = await response.json();
                // Redirect on success
                window.location.href = result.returnUrl || returnUrl || '/';
                return { success: true };
            } else {
                // Try to parse error message
                try {
                    const result = await response.json();
                    return { success: false, message: result.message || 'Giriş başarısız.' };
                } catch {
                    return { success: false, message: 'Sunucu hatası.' };
                }
            }
        } catch (error) {
            console.error('Login error:', error);
            return { success: false, message: 'Bağlantı hatası.' };
        }
    },

    register: async (url, data) => {
        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                const result = await response.json();
                window.location.href = result.returnUrl || '/';
                return { success: true };
            } else {
                try {
                    const result = await response.json();
                    // Validation errors might come as an object
                    if (result.errors) {
                        // Build a single error string for simplicity, or return object to handle in Blazor
                        // For now, let's just return the object and let Blazor handle it
                        return { success: false, errors: result }; // result IS the ValidationProblemDetails usually
                    }
                    return { success: false, message: result.message || 'Kayıt başarısız.' };
                } catch {
                    return { success: false, message: 'Sunucu hatası.' };
                }
            }
        } catch (error) {
            console.error('Register error:', error);
            return { success: false, message: 'Bağlantı hatası.' };
        }
    }
};
