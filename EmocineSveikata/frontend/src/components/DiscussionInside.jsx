import React, { useEffect, useState } from 'react';
import './DiscussionInside.css';
import { useParams } from 'react-router-dom';
import { fetchDiscussion } from '../api/discussionApi.js'

const DiscussionInside = () => {

  const { id } = useParams();

  const [discussion, setDiscussion] = useState('lump')

  useEffect(() => {
    const getData = async (id) => {
      const data = await fetchDiscussion(id);
      setDiscussion(data);
    };
    getData(id);
  }, []);

  return (
    <>
      <div className='discussion-container'>
        <div style={{ color: 'blue' }}>{discussion.title}</div>
      </div>
    </>
  )
}   

export default DiscussionInside;