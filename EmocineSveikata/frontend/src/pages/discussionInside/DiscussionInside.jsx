import React, { useEffect, useState } from 'react';
import Comment from './Comment'
import styles from './DiscussionInside.module.css';
import { useParams, useNavigate} from 'react-router-dom';
import { fetchDiscussion } from '../../api/discussionApi.js';
import { postCommentOnDiscussion } from '../../api/commentApi.js'
import DiscussionWidget from './DiscussionWidgets';
import { TextField, Divider, Button, Skeleton, Menu, MenuItem } from '@mui/material';
import { useAuth } from '../../contexts/AuthContext'
import MoreHorizIcon from '@mui/icons-material/MoreHoriz';


const DiscussionInside = () => {

  const { id } = useParams();

  const [discussion, setDiscussion] = useState(null)
  const [loading, setLoading] = useState(true);
  const [newCommentOpen, setNewCommentOpen] = useState(false);
  const [newComment, setNewComment] = useState('');
  const [anchorEl, setAnchorEl] = useState(null);
  const { currentUser } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const getData = async (id) => {
      const data = await fetchDiscussion(id);
      setDiscussion(data);
    };
    getData(id);
  }, []);

  useEffect(() => {
    setLoading(discussion == null)
  }, [discussion])

  const handleSubmit = async () => {
    const newData = await postCommentOnDiscussion(discussion.id, newComment)
    setNewComment('');
    setNewCommentOpen(false);
    setDiscussion(newData);
  }

  const handleMenuOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleEdit = () => {
    navigate(`/discussions/edit/${id}`);
    setAnchorEl(null);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  return (
    <>
      <div className={styles.discussionContainer}>
        {(loading)
          ? (
              <section className={styles.discussionCard}>
                <Skeleton variant="text" className={styles.skeletonTitle} />
                <Skeleton variant="text" className={styles.skeletonBody} />
                <Skeleton variant="text" className={styles.skeletonBody} />
                <Skeleton variant="text" className={styles.skeletonBody} />
              </section>
          ) : (
            <>
              <div className={styles.tags}>
                {discussion.tags.map((tag, index) => (
                  <span key={index} className={styles.tag}>
                    {tag}
                  </span>
                ))}
              </div>
              <section className={styles.discussionCard}>
                <div className={styles.discussionHeader}>
                  <span style={{ display: 'flex', alignItems: 'center' }}>
                    <img className={styles.authorPicture} src={discussion.authorPicture}/>
                    {discussion.authorName}
                  </span>
                  { currentUser && currentUser.user.id == discussion.authorId?
                    (
                      <>
                        <MoreHorizIcon className={styles.moreButton} onClick={handleMenuOpen} />
                        <Menu
                          anchorEl={anchorEl}
                          open={Boolean(anchorEl)}
                          onClose={handleMenuClose}
                        >
                          <MenuItem onClick={handleEdit}>Edit</MenuItem>
                        </Menu>
                      </>
                    ): null
                  }
                </div>
                <div style={{display: 'flex'}}>
                  <div className={styles.marginRight}>
                    <DiscussionWidget count={discussion.likes} discussionId={id} initialLiked={discussion.likedByUser} handleReply={() => setNewCommentOpen(!newCommentOpen)} />
                  </div>
                  <div className={styles.content}>
                    <h3 className={styles.discussionTitle}>{discussion.title}</h3>
                    <div>{discussion.content}</div>
                  </div>
                </div>
                {newCommentOpen ? (
                  <>
                    <Divider className={styles.horizontalDivider} />
                    <TextField
                      label="Comment"
                      multiline
                      rows={3}
                      variant="outlined"
                      onChange={(e) => setNewComment(e.target.value)}
                      fullWidth
                      sx={{ marginTop: '0.5em' }}
                    />
                    <Button
                      variant="contained"
                      disabled={!newComment}
                      sx={{ marginTop: '1em', backgroundColor: '#CB997E', '&:hover': { backgroundColor: 'rgba(203, 153, 126, 0.9)' } }}
                      onClick={handleSubmit}>
                      Submit
                    </Button>
                  </>
                ) : (
                  null
                )}
              </section>
            </>
          )
        }
        <p className={styles.negativeTopMargin}>Comments:</p>
        <section>
          {loading || discussion.comments.map((comment) => (
            <Comment key={comment.id} comment={comment} discussionId={id} />
          ))}
        </section>
      </div>
    </>
  )
}   

export default DiscussionInside;