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

const updateDiscussion = async (discussion) => {
  const token = JSON.parse(localStorage.getItem("user"))?.token;
  if (!token) throw new Error('Unauthorized');

  try {
    const response = await axios.put(DiscussionBaseUrl + discussion.id, {
      title: discussion.title,
      content: discussion.content,
      tags: discussion.tags,
      rowVersion: discussion.rowVersion
    }, {
      headers: {
        Authorization: `Bearer ${token}`
      },
      validateStatus: () => true
    });

    return response;

  } catch (error) {
    alert(error);
    console.error(error);
  }
}

const forceUpdateDiscussion = async (discussion) => {
  const token = JSON.parse(localStorage.getItem("user"))?.token;
  if (!token) throw new Error('Unauthorized');

  try {
    const response = await axios.put(DiscussionBaseUrl + discussion.id + '/force', {
      title: discussion.title,
      content: discussion.content,
      tags: discussion.tags,
      rowVersion: discussion.rowVersion
    }, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });

    return response;

  } catch (error) {
    alert(error);
    console.error(error);
  }
}

export { fetchDiscussion, likeDiscussion, updateDiscussion, forceUpdateDiscussion };