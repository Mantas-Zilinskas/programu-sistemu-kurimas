import React, { createContext, useContext, useState, useEffect } from 'react';

const DiscussionNewContext = createContext();

export const useDiscussionsNew = () => {
  return useContext(DiscussionNewContext);
};

export const DiscussionNewProvider = ({ children }) => {
  const [error, setError] = useState(null);

  const createDiscussion = async (discussionData) => {
    try {
      
      const response = await fetch('/api/discussions', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
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

  const value = {
    error,
    createDiscussion
  };

  return (
    <DiscussionNewContext.Provider value={value}>
      {children}
    </DiscussionNewContext.Provider>
  );
};
