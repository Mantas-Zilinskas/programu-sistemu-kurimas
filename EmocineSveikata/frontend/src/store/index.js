import { configureStore } from '@reduxjs/toolkit';
import discussionReducer from './slices/discussionSlice';

export const store = configureStore({
  reducer: {
    discussions: discussionReducer,
  },
});
