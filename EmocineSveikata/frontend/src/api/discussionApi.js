import { DiscussionBaseUrl } from './baseUrls'
import axios from 'axios';

const fetchDiscussion = async (id) => {
  try {
    const token = JSON.parse(localStorage.getItem("user"))?.token;

    const response = await axios.get(DiscussionBaseUrl + id, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    });

    return response.data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

export { fetchDiscussion };