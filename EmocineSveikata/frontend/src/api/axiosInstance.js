// src/api/axiosInstance.js
import axios from 'axios';

// Create an Axios instance with a custom configuration
const axiosInstance = axios.create({
    baseURL: 'http://your-backend-api-url',  // Replace with your API URL
    timeout: 5000,  // Timeout (optional)
    headers: {
        'Content-Type': 'application/json',
    },
});

export default axiosInstance;
