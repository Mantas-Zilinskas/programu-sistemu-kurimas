import React, { useEffect, useState } from 'react';
import Comment from './Comment'
import './DiscussionInside.css';
import { useParams } from 'react-router-dom';
import { fetchDiscussion } from '../../api/discussionApi.js';
import { postCommentOnDiscussion } from '../../api/commentApi.js'
import DiscussionWidget from './DiscussionWidgets';
import { TextField, Divider, Button, Skeleton } from '@mui/material';

const DiscussionInside = () => {

  const { id } = useParams();

  const [discussion, setDiscussion] = useState(null)
  const [loading, setLoading] = useState(true);
  const [newCommentOpen, setNewCommentOpen] = useState(false);
  const [newComment, setNewComment] = useState('');

  useEffect(() => {
    const getData = async (id) => {
      const data = await fetchDiscussion(id);
      setDiscussion(data);
    };
    getData(id);
  }, []);

  useEffect(() => {
    setLoading(discussion == null)
  }, [discussion])

  const handleSubmit = async () => {
    const newData = await postCommentOnDiscussion(discussion.id, newComment)
    setNewComment('');
    setNewCommentOpen(false);
    setDiscussion(newData);
  }

  return (
    <>
      <div className='discussion-container'>
        <section className='discussion-card'>
          {(loading)
            ? (
                <>
                  <Skeleton variant="text" className='skeleton-title' />
                  <Skeleton variant="text" className='skeleton-body' />
                  <Skeleton variant="text" className='skeleton-body' />
                  <Skeleton variant="text" className='skeleton-body' />
                </>
            ) : (
              <>
                <div style={{display: 'flex'}}>
                  <div className='margin-right'>
                    <DiscussionWidget count={discussion.likes} discussionId={id} handleReply={() => setNewCommentOpen(!newCommentOpen)} />
                  </div>
                  <div className='content'>
                    <h3 className='discussion-title'>{discussion.title}</h3>
                    <div>{discussion.content}</div>
                  </div>
                </div>
                {newCommentOpen ? (
                  <>
                    <Divider className='horizontal-divider' />
                    <TextField
                      label="Comment"
                      multiline
                      rows={3}
                      variant="outlined"
                      onChange={(e) => setNewComment(e.target.value)}
                      fullWidth
                      sx={{ marginTop: '0.5em' }}
                    />
                    <Button
                      variant="contained"
                      disabled={!newComment}
                      sx={{ marginTop: '1em', backgroundColor: '#CB997E', '&:hover': { backgroundColor: 'rgba(203, 153, 126, 0.9)' } }}
                      onClick={handleSubmit}>
                      Submit
                    </Button>
                  </>
                ) : (
                  null
                )}
              </>
            )
          }
        </section>
        <p className='negative-top-margin'>Comments:</p>
        <section>
          {loading || discussion.comments.map((comment) => (
            <Comment key={comment.id} comment={comment} discussionId={id} />
          ))}
        </section>
      </div>
    </>
  )
}   

export default DiscussionInside;