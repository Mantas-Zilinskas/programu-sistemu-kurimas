import { DiscussionBaseUrl } from './baseUrls'
import axios from 'axios';

const replyToComment = async (discussionId, commentId, content) => {
  try {
    const response = await axios.post(DiscussionBaseUrl + discussionId + '/comments/' + commentId + '/reply',
    {content: content});
    return response.data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

const postCommentOnDiscussion = async (discussionId, content) => {
  try {
    const response = await axios.post(DiscussionBaseUrl + discussionId + '/comments',
      { content: content });
    return response.data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

export { replyToComment, postCommentOnDiscussion };