import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';

// Mock data for discussions
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

// TODO: Implement actual API calls when backend is ready
export const fetchDiscussions = createAsyncThunk(
  'discussions/fetchDiscussions',
  async () => {
    // Simulate API call
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(mockDiscussions);
      }, 1000);
    });
  }
);

export const createDiscussion = createAsyncThunk(
  'discussions/createDiscussion',
  async (discussionData) => {
    // Simulate API call
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve({
          id: Date.now(),
          ...discussionData
        });
      }, 1000);
    });
  }
);

const initialState = {
  discussions: [],
  status: 'idle',
  error: null,
};

const discussionSlice = createSlice({
  name: 'discussions',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchDiscussions.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchDiscussions.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.discussions = action.payload;
      })
      .addCase(fetchDiscussions.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      })
      .addCase(createDiscussion.fulfilled, (state, action) => {
        state.discussions.push(action.payload);
      });
  },
});

export const selectDiscussions = (state) => state.discussions.discussions;
export const selectDiscussionStatus = (state) => state.discussions.status;
export const selectDiscussionError = (state) => state.discussions.error;

export default discussionSlice.reducer;
