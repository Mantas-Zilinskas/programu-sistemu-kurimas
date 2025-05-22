import axios from 'axios';

const fetchPositiveMessage = async (currentUser) => {
  try {
    const token = JSON.parse(localStorage.getItem("user"))?.token;
    const response = await axios.get(`/api/positiveMessages/${currentUser.user.id}/random`, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    });
    return response.data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

export { fetchPositiveMessage };
