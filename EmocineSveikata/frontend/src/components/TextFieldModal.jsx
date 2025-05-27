import React, { useState } from 'react';
import Modal from 'react-modal';
import styles from './TextFieldModal.module.css';
import TextField from '@mui/material/TextField';

const TextFieldModal = ({ isOpen, onRequestClose, handleSubmit, title}) => {

  const [content, setContent] = useState('');

  const handleButton = () => {
    if (content == '') return;
    handleSubmit(content);
    setContent('');
  }

  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      overlayClassName={styles.modalOverlay}
      className={styles.modalContent}
    >
      <h3 className={styles.centerText}>{title}</h3>
      <TextField
        label="Jūsų žinutė"
        multiline
        rows={4}
        value={content}
        onChange={(e) => setContent(e.target.value)}
        fullWidth
      />
      <br />
      <br />
      <div style={{ display: 'flex', justifyContent: 'center' }}>
        <button className={styles.button} onClick={handleButton}>Pateikti</button>
      </div>
    </Modal>
  );
};

export default TextFieldModal;
