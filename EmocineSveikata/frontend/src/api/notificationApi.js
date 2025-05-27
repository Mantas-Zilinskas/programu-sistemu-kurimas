import axios from 'axios';

export const fetchNotifications = async () => {
  const token = JSON.parse(localStorage.getItem("user"))?.token;
  if (!token) throw new Error('Unauthorized');

  const response = await axios.get('/api/Notification', {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data;
};

export const markNotificationsRead = async () => {
  const token = JSON.parse(localStorage.getItem("user"))?.token;
  if (!token) throw new Error('Unauthorized');

  await axios.post('/api/Notification/mark-read', {}, {
    headers: { Authorization: `Bearer ${token}` }
  });
};

export const sendNotification = async (id, message) => {
  const token = JSON.parse(localStorage.getItem("user"))?.token;
  if (!token) throw new Error('Unauthorized');

  await axios.post('/api/Notification/send',
    {
      id: id,
      message: message
    },
    {
      headers: { Authorization: `Bearer ${token}` }
  });
}