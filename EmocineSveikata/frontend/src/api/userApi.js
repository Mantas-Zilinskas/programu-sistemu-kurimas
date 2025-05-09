import axiosInstance from './axiosInstance';  // Ensure axiosInstance is correctly imported

// Function to get the user profile
export const getUserProfile = async (token) => {
    try {
        const response = await axiosInstance.get('/Auth/profile', {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching user profile:', error);
        throw error;
    }
};

// Export updateUserProfile if not already
export const updateUserProfile = async (token, userData) => {
    try {
        const response = await axiosInstance.put('/Auth/profile', userData, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error updating profile:', error);
        throw error;
    }
};
