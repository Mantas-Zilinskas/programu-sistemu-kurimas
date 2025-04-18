import React, { useState } from 'react';
import CommentWidget from './CommentWidget';
import { TextField, Divider, Button } from '@mui/material';
import { replyToComment } from '../../api/commentApi.js';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import './Comment.css';

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
      return (<KeyboardArrowUpIcon onClick={handleExpandReplies} className='pointer-icon' />)
    } else {
      return (<KeyboardArrowDownIcon onClick={handleExpandReplies} className='pointer-icon' />)
    }
  }

  return (
    <>
      <div className='comment-card'>
        <div style={{ display: 'flex' }}>
          <div className='margin-right'>
            <CommentWidget
              count={commentData.likes}
              CommentId={commentData.likes}
              handleReply={() => { setNewReplyOpen(!newReplyOpen) }}
              renderFunction={renderExpandRepliesButton} />
          </div>
          <p>{commentData.content}</p>
        </div>
          {newReplyOpen ? (
          <>
            <Divider className='horizontal-divider' />
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
              <Comment comment={reply} discussionId={discussionId} />
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