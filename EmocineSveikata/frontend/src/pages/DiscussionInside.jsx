import React, { useEffect, useState } from 'react';
import Comment from '../components/Comment'
import './DiscussionInside.css';
import { useParams } from 'react-router-dom';
import { fetchDiscussion } from '../api/discussionApi.js'
import DiscussionWidget from '../components/DiscussionWidgets';
import Skeleton from '@mui/material/Skeleton';

const DiscussionInside = () => {

  const { id } = useParams();

  const [discussion, setDiscussion] = useState(null)
  const [loading, setLoading] = useState(true);


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
              <div style={{display: 'flex'}}>
                <div className='margin-right'>
                  <DiscussionWidget count={discussion.likes} discussionId={id} loading={loading} />
                </div>
                <div className='content'>
                  <h3 className='discussion-title'>{discussion.title}</h3>
                  <div>{discussion.content}</div>
                </div>
              </div>
            )
          }
        </section>
        <p className='negative-top-margin'>Comments:</p>
        <section>
          {loading || discussion.comments.map((comment) => (
              <Comment key={comment.id} comment={comment} />
          ))}
        </section>
      </div>
    </>
  )
}   

export default DiscussionInside;