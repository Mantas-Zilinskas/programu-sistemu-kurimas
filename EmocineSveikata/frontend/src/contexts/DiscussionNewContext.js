import React, { createContext, useContext, useState, useEffect } from 'react';

const DiscussionNewContext = createContext();

export const useDiscussionsNew = () => {
  return useContext(DiscussionNewContext);
};

export const DiscussionNewProvider = ({ children }) => {
  const [error, setError] = useState(null);
  const [tagsArray, setTagsArray] = useState([]);

  useEffect(() => {
    fetchTags();
  }, []);

  const createDiscussion = async (discussionData) => {
	const token = JSON.parse(localStorage.getItem("user"))?.token;
    try {
      
      const response = await fetch('/api/discussions', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
		  'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(discussionData),
      });
      const data = await response.json();
      return data;
    } catch (err) {
      setError('Failed to create discussion');
      throw err;
    }
  };
  
  const fetchTags = async () => {
    try {
      
      const response = await fetch('/api/discussions/tags');
      const data = await response.json();
      setTagsArray(data);
    } catch (err) {
      setError('Failed to fetch tags');
    }
  };

  const value = {
    tagsArray,
    setTagsArray,
    error,
    createDiscussion
  };

  return (
    <DiscussionNewContext.Provider value={value}>
      {children}
    </DiscussionNewContext.Provider>
  );
};
