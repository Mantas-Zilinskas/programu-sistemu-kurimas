import React from 'react';
import { DiscussionProvider } from './contexts/DiscussionContext';
import Discussions from './components/Discussions';

function App() {
    return (
        <DiscussionProvider>
            <Discussions />
        </DiscussionProvider>
    );
}

export default App;
