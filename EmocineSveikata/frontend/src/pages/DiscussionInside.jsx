import React, { useEffect, useState } from 'react';
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
        
        <div className='discussion-card'>
          {(loading)
            ? (
                <>
                  <Skeleton variant="text" style={{ width: '60%', fontSize: '1.5em'}} />
                  <Skeleton variant="rounded" style={{ width: '100%', height: '100px'}} />
                </>
          ) : (
              <div style={{display: 'flex'}}>
                <div className='small-box'>
                  <DiscussionWidget count={discussion.likes} id={5} loading={loading} />
                </div>
                <div className='content'>
                  <h3 className='discussion-title'>{discussion.title}</h3>
                  <div>{discussion.content}</div>
                </div>
              </div>
            )
          }
        </div>
      </div>
    </>
  )
}   

export default DiscussionInside;