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
  const [tagsArray, setTagsArray] = useState([]);
  const [selectedTag, setSelectedTag] = useState('');
  const [isPopular, setIsPopular] = useState(false);

  useEffect(() => {
    fetchTags();
  }, []);

  useEffect(() => {
    fetchDiscussions(selectedTag, isPopular);
  }, [selectedTag, isPopular]);

  const fetchDiscussions = async (tag, isPopular) => {
    setLoading(true);
    try {
      const url = tag
        ? `/api/discussions?tag=${encodeURIComponent(tag)}&isPopular=${encodeURIComponent(isPopular)}`
        : `/api/discussions?isPopular=${encodeURIComponent(isPopular)}`;
      const response = await fetch(url);
      const data = await response.json();
      setDiscussions(data);
      setLoading(false);
    } catch (err) {
      setError('Failed to fetch discussions');
      setLoading(false);
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
    discussions,
    tagsArray,
    selectedTag,
    setSelectedTag,
    isPopular,
    setIsPopular,
    loading,
    error
  };

  return (
    <DiscussionContext.Provider value={value}>
      {children}
    </DiscussionContext.Provider>
  );
};
