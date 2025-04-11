// DiscussionContext.js
import React, { createContext, useContext, useState, useEffect } from 'react';

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

export const useDiscussions = () => useContext(DiscussionContext);

export const DiscussionProvider = ({ children }) => {
    const [discussions, setDiscussions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const loadDiscussions = () => {
        try {
            const saved = localStorage.getItem('discussions');
            if (saved) {
                setDiscussions(JSON.parse(saved));
            } else {
                setDiscussions(mockDiscussions);
                localStorage.setItem('discussions', JSON.stringify(mockDiscussions));
            }
        } catch (err) {
            setError('Nepavyko įkrauti diskusijų.');
        }
        setLoading(false);
    };

    useEffect(() => {
        loadDiscussions();
    }, []);

    const createDiscussion = (discussionData) => {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                try {
                    const newDiscussionWithId = {
                        id: Date.now(), // generates a unique id
                        ...discussionData
                    };
                    const updatedDiscussions = [...discussions, newDiscussionWithId];
                    setDiscussions(updatedDiscussions);
                    localStorage.setItem('discussions', JSON.stringify(updatedDiscussions));
                    resolve(newDiscussionWithId);
                } catch (err) {
                    setError('Nepavyko sukurti diskusijos.');
                    reject(err);
                }
            }, 1000);
        });
    };

    return (
        <DiscussionContext.Provider value={{ discussions, loading, error, createDiscussion }}>
            {children}
        </DiscussionContext.Provider>
    );
};
