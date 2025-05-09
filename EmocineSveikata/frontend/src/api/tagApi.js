import { DiscussionBaseUrl } from './baseUrls'
import axios from 'axios';

const fetchTags = async () => {
  try {
    const response = await axios.get(`${DiscussionBaseUrl}/tags`);
    return response.data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

export { fetchTags };