import React, { createContext, useContext, useState, useEffect } from 'react';

// Mock data
/*
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
*/

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
      
      const response = await fetch('/api/discussions?pageSize=100'); // TODO pakeisti į puslapius ar infinite scroll
      const data = await response.json();
      setDiscussions(data);
      setLoading(false);
    } catch (err) {
      setError('Failed to fetch discussions');
      setLoading(false);
    }
  };

  const value = {
    discussions,
    loading,
    error,
    fetchDiscussions
  };

  return (
    <DiscussionContext.Provider value={value}>
      {children}
    </DiscussionContext.Provider>
  );
};
