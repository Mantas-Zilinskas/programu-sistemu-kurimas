import { DiscussionBaseUrl } from './baseUrls'
import axios from 'axios';

const replyToComment = async (discussionId, commentId, content) => {
  const token = JSON.parse(localStorage.getItem("user"))?.token;
  try {
    const response = await axios.post(DiscussionBaseUrl + discussionId + '/comments/' + commentId + '/reply',
    {content: content},
    { headers: 
      { Authorization: `Bearer ${token}`}
    });
    return response.data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

const postCommentOnDiscussion = async (discussionId, content) => {
  const token = JSON.parse(localStorage.getItem("user"))?.token;
  try {
    const response = await axios.post(DiscussionBaseUrl + discussionId + '/comments',
      { content: content },
      { headers: 
        { Authorization: `Bearer ${token}`}
      });
    return response.data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

const likeComment = async (discussionId, commentId) => {
    const token = JSON.parse(localStorage.getItem("user"))?.token;
    if (!token) throw new Error('Unauthorized');
  
    try {
      const response = await axios.post(
        `${DiscussionBaseUrl}${discussionId}/comments/${commentId}/like`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
          }
        }
      );
      return response.data;
    } catch (error) {
      console.error('Error liking comment:', error);
      throw error;
    }
  };
  
export { replyToComment, postCommentOnDiscussion, likeComment};