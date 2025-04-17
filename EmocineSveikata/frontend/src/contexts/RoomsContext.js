import React, { createContext, useContext, useState, useEffect } from 'react';

const RoomsContext = createContext();

export const useRooms = () => {
  return useContext(RoomsContext);
};

export const RoomsProvider = ({ children }) => {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchRooms();
  }, []);

  const fetchRooms = async () => {
    setLoading(true);
    try {
      // TODO: API
      // const response = await fetch('/api/rooms');
      // const data = await response.json();
      // setRooms(data);
      setLoading(false);
    } catch (err) {
      setError('Failed to fetch rooms');
      setLoading(false);
    }
  };

  const value = {
    rooms,
    loading,
    error
  };

  return (
    <RoomsContext.Provider value={value}>
      {children}
    </RoomsContext.Provider>
  );
};
