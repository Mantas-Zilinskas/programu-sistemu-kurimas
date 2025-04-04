import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  Container,
  Typography,
  Card,
  CardContent,
  TextField,
  Button,
  CircularProgress,
  Box,
} from '@mui/material';
import {
  fetchDiscussions,
  createDiscussion,
  selectDiscussions,
  selectDiscussionStatus,
  selectDiscussionError,
} from '../store/slices/discussionSlice';

const Discussions = () => {
  const dispatch = useDispatch();
  const discussions = useSelector(selectDiscussions);
  const status = useSelector(selectDiscussionStatus);
  const error = useSelector(selectDiscussionError);
  const [newDiscussion, setNewDiscussion] = useState({ title: '', content: '' });

  useEffect(() => {
    if (status === 'idle') {
      dispatch(fetchDiscussions());
    }
  }, [status, dispatch]);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (newDiscussion.title && newDiscussion.content) {
      dispatch(createDiscussion(newDiscussion));
      setNewDiscussion({ title: '', content: '' });
    }
  };

  if (status === 'loading') {
    return (
      <Box display="flex" justifyContent="center" mt={4}>
        <CircularProgress />
      </Box>
    );
  }

  if (status === 'failed') {
    return (
      <Typography color="error" align="center" mt={4}>
        Error: {error}
      </Typography>
    );
  }

  return (
    <Container maxWidth="md">
      <Typography variant="h4" component="h1" gutterBottom mt={4}>
        Diskusijos
      </Typography>

      <Card sx={{ mb: 4 }}>
        <CardContent>
          <form onSubmit={handleSubmit}>
            <TextField
              fullWidth
              label="Pavadinimas"
              value={newDiscussion.title}
              onChange={(e) =>
                setNewDiscussion({ ...newDiscussion, title: e.target.value })
              }
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Turinys"
              value={newDiscussion.content}
              onChange={(e) =>
                setNewDiscussion({ ...newDiscussion, content: e.target.value })
              }
              margin="normal"
              multiline
              rows={4}
              required
            />
            <Button
              type="submit"
              variant="contained"
              color="primary"
              sx={{ mt: 2 }}
            >
              Sukurti diskusiją
            </Button>
          </form>
        </CardContent>
      </Card>

      {discussions.map((discussion) => (
        <Card key={discussion.id} sx={{ mb: 2 }}>
          <CardContent>
            <Typography variant="h6" gutterBottom>
              {discussion.title}
            </Typography>
            <Typography variant="body1" color="text.secondary">
              {discussion.content}
            </Typography>
          </CardContent>
        </Card>
      ))}
    </Container>
  );
};

export default Discussions;
