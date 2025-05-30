import React, { useState } from 'react';
import Modal from 'react-modal';
import styles from './YesNoModal.module.css';

const YesNoModal = ({ isOpen, onRequestClose, content, handleYes, handleNo, middleText=null, handleMiddle=null }) => {

  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      overlayClassName={styles.yesNoModalOverlay}
      className={styles.yesNoModalContent}     
    >
      <h3 className={styles.centerText}>{content}</h3>
      <br />
      <div style={{ display: 'flex', justifyContent: 'space-between' }}>
        <button className={styles.button} onClick={handleYes}>Taip</button>
        {middleText && handleMiddle && <button className={styles.wideButton} onClick={handleMiddle}>{middleText}</button>}
        <button className={styles.button} onClick={handleNo}>Ne</button>
      </div>
    </Modal>
  );
};

export default YesNoModal;
