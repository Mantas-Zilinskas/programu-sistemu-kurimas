import { DiscussionBaseUrl } from './baseUrls'
import axios from 'axios';

const fetchDiscussion = async (id) => {
  try {
    const response = await axios.get(DiscussionBaseUrl + id);
    return response.data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

export { fetchDiscussion };