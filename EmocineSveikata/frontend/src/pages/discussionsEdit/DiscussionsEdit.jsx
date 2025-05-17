import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { fetchDiscussion, updateDiscussion, forceUpdateDiscussion } from '../../api/discussionApi.js';
import { fetchTags } from '../../api/tagApi.js';
import { useAuth } from '../../contexts/AuthContext.jsx'
import YesNoModal from '../../components/YesNoModal.jsx'
import './DiscussionsEdit.css';

const DiscussionsEdit = () => {
  const { id } = useParams();
  const [discussion, setDiscussion] = useState(null);
  const [currentTag, setCurrentTag] = useState("");
  const [tags, setTags] = useState([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const { currentUser } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const getData = async (id) => {
      const discussion = await fetchDiscussion(id);
      const tags = await fetchTags();
      setDiscussion(discussion);
      setTags(tags);
    };
    getData(id);
  }, [])

  useEffect(() => {
    let isLoading = (discussion != null)
    if (isLoading) {
      if (currentUser.user.id != discussion.authorId) navigate('/')
      setLoading(!isLoading);
    }
  }, [discussion])

  const handleAddTag = () => {
    if (currentTag && tags.includes(currentTag)) {
      setDiscussion(prevState => ({
        ...prevState,
        tags: [...prevState.tags, currentTag]
      }));
      setTags(tags.filter((tag) => tag !== currentTag));
      setCurrentTag("");
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (discussion.title && discussion.content) {
      setLoading(true);
      let response = await updateDiscussion(discussion);

      if (response.status == 200) {
        navigate(`/discussions/${id}`);
      } else if (response.status == 409) {
        setIsModalOpen(true);
      } else {
        alert('Something went wrong while trying to update discussion');
        navigate(`/discussions/${id}`);
      }
    }
  };

  const handleNo = () => {
    navigate(`/discussions/${id}`);
  }

  const handleYes = async () => {
    if (discussion.title && discussion.content) {
      await forceUpdateDiscussion(discussion);
      navigate(`/discussions/${id}`);
    }
  }

  if (loading) {
    return (
      <div className="loading-container">
        <div className="loading-spinner"></div>
        <YesNoModal
          isOpen={isModalOpen}
          onRequestClose={() => { setIsModalOpen(false) }}
          content="Kažkas jau redagavo šią diskusiją. Ar norėtumėt vistiek išsaugoti savo pakeitimus?"
          handleNo={handleNo}
          handleYes={handleYes}
        />
      </div>
    );
  }

  return ((discussion && (
    <div className="new-discussion-container">
      <h1 className="new-discussion-title">Redaguoti diskusiją</h1>
      <div className="new-discussion-form-card">
        <form className="new-discussion-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="title" className="form-label">Pavadinimas</label>
            <input
              id="title"
              type="text"
              className="form-input"
              value={discussion?.title || ""}
              onChange={(e) =>
                setDiscussion({ ...discussion, title: e.target.value })
              }
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="content" className="form-label">Turinys</label>
            <textarea
              id="content"
              className="form-textarea"
              value={discussion?.content || ""}
              onChange={(e) =>
                setDiscussion({ ...discussion, content: e.target.value })
              }
              rows={4}
              required
            ></textarea>
          </div>
          <div className="form-group">
            <label htmlFor="tags" className="form-label">Žymos</label>
            <div className="tag-select-container">
              <select
                id="tags"
                className="tag-select"
                value={currentTag}
                onChange={(e) => setCurrentTag(e.target.value)}
              >
                <option value="" disabled>Pasirinkti žymą</option>
                {tags.map((tag) => (
                  <option key={tag} value={tag}>{tag}</option>
                ))}
              </select>
              <button type="button" className="add-tag-button" onClick={handleAddTag}>+</button>
            </div>
            <div className="selected-tags-wrapper">
              <label htmlFor="selected-tags-label" className="form-label">Pasirinktos žymos:</label>
              <div className="selected-tags">
                {discussion.tags.map((tag) => (
                  <span key={tag} className="tag-chip">{tag}</span>
                ))}
              </div>
            </div>
          </div>
          <button
            type="submit"
            className="submit-button"
          >
            Išsaugoti diskusiją
          </button>
        </form>
      </div>
    </div>
  )))
};

export default DiscussionsEdit;