import React, { useState, useEffect } from 'react';
import CommentWidget from './CommentWidget';
import { TextField, Divider, Button } from '@mui/material';
import { replyToComment } from '../../api/commentApi.js';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import commentStyles from './Comment.module.css';
import styles from './DiscussionInside.module.css';

const Comment = ({comment, discussionId}) => {

  const [commentData, setCommentData] = useState(comment);
  const [newReplyOpen, setNewReplyOpen] = useState(false);
  const [newReply, setNewReply] = useState('');
  const [repliesOpen, setRepliesOpen] = useState(false);

  const handleSubmit = async () => {
    let newData = await replyToComment(discussionId, commentData.id, newReply);
    newData = newData.replies[0];
    setNewReply('');
    setNewReplyOpen(false);
    const commentDataClone = structuredClone(commentData);
    commentDataClone.replies.push(newData);
    setCommentData(commentDataClone);
  }

  const handleExpandReplies = () => {
    setRepliesOpen(!repliesOpen);
  }

  const renderExpandRepliesButton = () => {
    if (commentData.replies.length == 0) {
      return null
    } else if (repliesOpen) {
      return (<KeyboardArrowUpIcon onClick={handleExpandReplies} className={commentStyles.pointerIcon} />)
    } else {
      return (<KeyboardArrowDownIcon onClick={handleExpandReplies} className={commentStyles.pointerIcon} />)
    }
  }

  return (
    <>
      <div className={commentStyles.commentCard}>
        <div className={styles.discussionHeader}>
          <span style={{ display: 'flex', alignItems: 'center' }}>
            <img className={styles.authorPicture} src={comment.authorPicture} />
            {comment.authorName}
          </span>
        </div>
        <div style={{ display: 'flex' }}>
          <div className={styles.marginRight}>
            <CommentWidget
              count={commentData.likes}
			  DiscussionId={discussionId}
              CommentId={commentData.id}
			  initialLiked={commentData.likedByUser}
              handleReply={() => { setNewReplyOpen(!newReplyOpen) }}
              renderFunction={renderExpandRepliesButton} />
          </div>
          <p>{commentData.content}</p>
        </div>
          {newReplyOpen ? (
          <>
            <Divider className={commentStyles.horizontalDivider} />
            <TextField
              label="Reply"
              multiline
              rows={3}
              variant="outlined"
              onChange={(e) => setNewReply(e.target.value)}
              fullWidth
              sx={{ marginTop: '0.5em' }}
            />
            <Button
              variant="contained"
              disabled={!newReply}
              sx={{ margin: '1em 0', backgroundColor: '#CB997E', '&:hover': { backgroundColor: 'rgba(203, 153, 126, 0.9)' } }}
              onClick={handleSubmit}>
              Submit
            </Button>
          </>
        ) : (
          null
        )}
        {repliesOpen ?
          commentData.replies.map((reply) => ( 
            <>
              <Comment key={reply.id} comment={reply} discussionId={discussionId} />
            </>
          ))
          :
          null
        }
      </div>
    </>
  )
}

export default Comment;