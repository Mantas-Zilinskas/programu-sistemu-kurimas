import axios from 'axios';

const fetchAvailableRooms = async () => {
  try {
    const response = await axios.get('/api/Room/available');
    return response.data;
  } catch (error) {
    console.error('Error fetching rooms:', error);
    return null;
  }
};

const bookRoom = async (roomId) => {
  const token = JSON.parse(localStorage.getItem("user"))?.token;
  if (!token) throw new Error('Unauthorized');
  
  try {
    const response = await axios.post(
      `/api/Room/book/${roomId}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      }
    );
    return response.data;
  } catch (error) {
    console.error('Error booking room:', error);
    throw error;
  }
};

export { fetchAvailableRooms, bookRoom };
