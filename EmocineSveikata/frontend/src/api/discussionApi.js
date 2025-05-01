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
    return error;
  }
}

<<<<<<< HEAD
const likeDiscussion = async (discussionId) => {
    const token = JSON.parse(localStorage.getItem("user"))?.token;
    if (!token) throw new Error('Unauthorized');
  
    try {
      const response = await axios.post(
        `${DiscussionBaseUrl}${discussionId}/like`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
          }
        }
      );
      return response.data;
    } catch (error) {
      console.error('Error liking discussion:', error);
      throw error;
    }
  };

export { fetchDiscussion, likeDiscussion };
=======
const updateDiscussion = async (discussion) => {
  try {
    const response = await axios.put(DiscussionBaseUrl + discussion.id, discussion);
    return response;
  } catch (error) {
    console.error('Error:', error);
    return error;
  }
}

export { fetchDiscussion, updateDiscussion };
>>>>>>> 41ac4f2 (PSK-40 can now eddit discussions)
