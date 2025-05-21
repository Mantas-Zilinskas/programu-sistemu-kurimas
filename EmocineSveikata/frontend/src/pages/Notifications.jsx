import React, { useEffect, useState } from 'react';
import { fetchNotifications, markNotificationsRead } from '../api/notificationApi';
import { useNavigate } from 'react-router-dom';
import styles from './Notifications.module.css';

const NotificationsPage = () => {
  const [notifications, setNotifications] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const loadNotifications = async () => {
      try {
        const data = await fetchNotifications();
        setNotifications(data);
		await markNotificationsRead();
      } catch (err) {
        console.error('Failed to load notifications', err);
      }
    };
    loadNotifications();
  }, []);

  return (
	<div className={styles.notificationList}>
	  <h2>Your Notifications</h2>
	  {notifications.length === 0 ? (
		<p>No notifications yet.</p>
	  ) : (
		notifications.map((n) => (
		  <div
			key={n.id}
			onClick={() => navigate(`/${n.link}`)}
			className={`${styles.notificationCard} ${!n.isRead ? styles.unread : ''}`}
		  >
			<span className={styles.notificationMessage}>{n.message}</span>
			<span className={styles.notificationDate}>
			  {new Date(n.createdAt).toLocaleString()}
			</span>
		  </div>
		))
	  )}
	</div>
  );  
};

export default NotificationsPage;