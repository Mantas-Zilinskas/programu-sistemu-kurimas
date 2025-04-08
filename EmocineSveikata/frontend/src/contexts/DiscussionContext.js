import React, { createContext, useContext, useState, useEffect } from 'react';

// Mock data 
const mockDiscussions = [
  {
    id: 1,
    title: 'Kaip įveikti stresą?',
    content: 'Pasidalinkite savo patirtimi, kaip jūs tvarkotės su stresu kasdieniniame gyvenime.'
  },
  {
    id: 2,
    title: 'Meditacijos nauda',
    content: 'Kokią naudą pastebėjote praktikuodami meditaciją?'
  }
];

const DiscussionContext = createContext();

export const useDiscussions = () => {
  return useContext(DiscussionContext);
};

export const DiscussionProvider = ({ children }) => {
  const [discussions, setDiscussions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchDiscussions();
  }, []);

  const fetchDiscussions = async () => {
    setLoading(true);
    try {
      //  API simuliacija
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // pvz veliau bus naudojamas kaip fetch call'as backend API
      // const response = await fetch('/api/discussions');
      // const data = await response.json();
      // setDiscussions(data);
      
      setDiscussions(mockDiscussions);
      setLoading(false);
    } catch (err) {
      setError('Failed to fetch discussions');
      setLoading(false);
    }
  };


  const createDiscussion = async (discussionData) => {
    try {
      //API simuliacija 
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // pvz veliau bus kaip POST request i backend API
      // const response = await fetch('/api/discussions', {
      //   method: 'POST',
      //   headers: {
      //     'Content-Type': 'application/json',
      //   },
      //   body: JSON.stringify(discussionData),
      // });
      // const data = await response.json();
      
      const newDiscussionWithId = {
        id: Date.now(),
        ...discussionData
      };
      
      setDiscussions(prevDiscussions => [...prevDiscussions, newDiscussionWithId]);
      return newDiscussionWithId;
    } catch (err) {
      setError('Failed to create discussion');
      throw err;
    }
  };

  const value = {
    discussions,
    loading,
    error,
    fetchDiscussions,
    createDiscussion
  };

  return (
    <DiscussionContext.Provider value={value}>
      {children}
    </DiscussionContext.Provider>
  );
};
