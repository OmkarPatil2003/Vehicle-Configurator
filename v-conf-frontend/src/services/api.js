/**
 * Purpose: Central Axios instance configuration with interceptors for JWT token management and error handling.
 */
import axios from 'axios';

const api = axios.create({
    baseURL: '/api',
    headers: {
        'Content-Type': 'application/json',
    },
});

// Request interceptor to add JWT token
api.interceptors.request.use(
    (config) => {
        const token = sessionStorage.getItem('access_token'); // Changed to sessionStorage as per requirement
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Response interceptor to handle auth errors
api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response) {
            // Unauthenticated
            if (error.response.status === 401) {
                sessionStorage.removeItem('access_token'); // Changed to sessionStorage as per requirement
                // window.location.href = '/login'; 
            }
            // Server Error (500)
            else if (error.response.status === 500) {
                const message = encodeURIComponent(error.response.data?.message || "Internal Server Error");
                window.location.href = `/error?message=${message}`;
            }
        } else if (error.request) {
            // Network Error (No response received)
            const message = encodeURIComponent("Network Error: Unable to connect to server.");
            window.location.href = `/error?message=${message}`;
        }

        return Promise.reject(error);
    }
);

export default api;
