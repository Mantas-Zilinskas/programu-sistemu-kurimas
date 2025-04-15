import React, { useEffect, useState } from 'react';
import CommentWidget from './CommentWidget'
import './Comment.css';

const Comment = ({comment}) => {

  return (
    <>
      <div className='comment-card'>
        <div style={{ display: 'flex' }}>
          <div className='margin-right'>
            <CommentWidget count={comment.likes} CommentId={comment.likes} />
          </div>
          <p>{comment.content}</p>
        </div>
      </div>
    </>
  )
}

export default Comment;