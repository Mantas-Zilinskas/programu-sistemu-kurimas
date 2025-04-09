import React from 'react';
import { Link } from 'react-router-dom';
import './Home.css';

const Home = () => {
  return (
    <div className="home-container">
      <div className="home-section">
        <div className="home-hero">
          <h1 className="home-title">Emocinė Sveikata</h1>
          <p className="home-subtitle">
            Platforma, skirta psichologinei gerovei stiprinti ir emocinei sveikatai palaikyti
          </p>
          <Link to="/discussions" className="home-button">
            Prisijungti prie diskusijų
          </Link>
        </div>

        <div className="home-grid">
          <div className="home-card">
            <h2 className="home-card-title">Diskusijos ir palaikymas</h2>
            <p className="home-card-content">
              Dalinkitės savo patirtimi, užduokite klausimus ir gaukite palaikymą 
              iš bendraminčių. Diskutuokite apie emocines problemas saugioje aplinkoje.
            </p>
          </div>
          <div className="home-card">
            <h2 className="home-card-title">Emocinė pagalba</h2>
            <p className="home-card-content">
              Sužinokite apie savipagalbos metodus, streso valdymo technikas 
              ir būdus, kaip rūpintis savo psichologine gerove kasdien.
            </p>
          </div>
          <div className="home-card">
            <h2 className="home-card-title">Profesionalų patarimai</h2>
            <p className="home-card-content">
              Gaukite vertingų įžvalgų ir patarimų iš psichologijos specialistų 
              įvairiomis emocinės sveikatos temomis.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;
