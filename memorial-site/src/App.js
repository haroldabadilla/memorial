import React, { useState, useEffect } from 'react';
import './App.css';

/**
 * ============================================================================
 * ICONS & ASSETS
 * ============================================================================
 */
const IconCSharp = () => (
  <img
    src="./icons/csharp-icon.png"
    alt="C# .NET"
    className="tech-icon-img"
    title="Backend Data Generation"
  />
);

const IconReact = () => (
  <svg viewBox="-11.5 -10.23174 23 20.46348" className="tech-icon" title="React" aria-hidden="true">
    <circle cx="0" cy="0" r="2.05" fill="#61dafb" />
    <g stroke="#61dafb" strokeWidth="1" fill="none">
      <ellipse rx="11" ry="4.2" />
      <ellipse rx="11" ry="4.2" transform="rotate(60)" />
      <ellipse rx="11" ry="4.2" transform="rotate(120)" />
    </g>
  </svg>
);

/**
 * ============================================================================
 * SUB-COMPONENTS
 * ============================================================================
 */

// 1. Navigation Bar with Mobile Hamburger Menu
const Navbar = () => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleMenu = () => setIsOpen(!isOpen);
  const closeMenu = () => setIsOpen(false);

  return (
    <nav className="navbar">
      <h1>Remembering Redempta</h1>

      {/* Mobile Hamburger Icon */}
      <div className="hamburger" onClick={toggleMenu} aria-label="Toggle navigation">
        <span className={`bar ${isOpen ? 'animate' : ''}`}></span>
        <span className={`bar ${isOpen ? 'animate' : ''}`}></span>
        <span className={`bar ${isOpen ? 'animate' : ''}`}></span>
      </div>

      {/* Navigation Links */}
      <div className={`nav-links ${isOpen ? 'active' : ''}`}>
        <a href="#" onClick={closeMenu}>Home</a>
        <a href="#message" onClick={closeMenu}>Message</a>
        <a href="#tributes" onClick={closeMenu}>Tributes</a>
        {/* FIXED: Added Gallery Link */}
        <a href="#gallery" onClick={closeMenu}>Gallery</a>
      </div>
    </nav>
  );
};

// 2. Edge-to-Edge Hero Section
const HeroSection = () => (
  <header className="hero-section-cinema">
    <div className="hero-content-wrapper">
      <img
        src="./hero-image.jpg"
        alt="In Loving Memory"
        className="hero-image-cinema"
      />

      {/* Scroll Indicator */}
      <div className="scroll-indicator">
        <span>Scroll for Message</span>
        <div className="arrow-down"></div>
      </div>
    </div>
  </header>
);

// 3. Family Message Section
const FamilyMessage = ({ message }) => {
  const paragraphs = message ? message.split('\n') : [];

  return (
    <section id="message" className="family-message-section">
      <div className="message-container">
        <h2 className="script-font">A Message from the Family</h2>

        {paragraphs.map((text, index) => (
          text.trim() && (
            <p key={index} className="message-body">
              {text}
            </p>
          )
        ))}

        <div className="separator-line"></div>
      </div>
    </section>
  );
};

// 4. Modal for Viewing Photos
const ImageModal = ({ image, onClose }) => {
  if (!image) return null;

  // Cloudinary Optimization: Request High-Res (1200px) for Modal
  const highResImage = image.includes("cloudinary")
    ? image.replace("f_auto,q_auto", "w_1200,q_90")
    : image;

  return (
    <div className="modal" onClick={onClose}>
      <button className="close-btn" onClick={onClose} aria-label="Close">&times;</button>
      <img
        src={highResImage}
        alt="Full resolution memory"
        className="modal-img"
        onClick={(e) => e.stopPropagation()}
      />
    </div>
  );
};

/**
 * ============================================================================
 * MAIN APPLICATION
 * ============================================================================
 */
function App() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [selectedImage, setSelectedImage] = useState(null);

  useEffect(() => {
    fetch('./memorial_data.json')
      .then(res => res.json())
      .then(jsonData => {
        setData(jsonData);
        setLoading(false);
      })
      .catch(err => console.error("Failed to load data:", err));
  }, []);

  if (loading) return <div className="state-message">Loading Memorial...</div>;

  return (
    <div className="app-container">
      <Navbar />

      <HeroSection />

      <FamilyMessage message={data.headerMessage} />

      <main className="container">

        {/* Tributes (Videos) */}
        <section id="tributes" className="card">
          <h2 className="section-title">Ceremony & Tributes</h2>
          <div className="tribute-list">
            {data.videos?.map((video) => (
              <article key={video.id} className="tribute-item">
                <h3 className="video-title">{video.title}</h3>
                <div className="video-wrapper">
                  <iframe
                    title={video.title}
                    src={`https://www.youtube.com/embed/${video.youtubeId}`}
                    frameBorder="0" allowFullScreen loading="lazy"
                  />
                </div>
              </article>
            ))}
          </div>
        </section>

        {/* Gallery (Photos) */}
        <section id="gallery" className="card">
          <h2 className="section-title">Life in Pictures</h2>
          <div className="gallery-grid">
            {data.photos?.map((photo) => {
              // Cloudinary Optimization: Request Thumbnail (300px square) for Grid
              const thumbUrl = photo.url.includes("cloudinary")
                ? photo.url.replace("upload/", "upload/w_300,c_fill,ar_1:1/")
                : photo.url;

              return (
                <img
                  key={photo.id}
                  src={thumbUrl}
                  alt={photo.title}
                  className="gallery-img"
                  loading="lazy"
                  onClick={() => setSelectedImage(photo.url)}
                />
              );
            })}
          </div>
        </section>

      </main>

      {/* Footer */}
      <footer className="footer">
        <p className="footer-msg">{data.footerMessage}</p>

        <div className="tech-stack-container">
          <IconCSharp />
          <IconReact />
        </div>

        <small className="copyright">
          &copy; {new Date().getFullYear()} In Loving Memory. All Rights Reserved.
        </small>
      </footer>

      {/* Popup Modal */}
      <ImageModal image={selectedImage} onClose={() => setSelectedImage(null)} />
    </div>
  );
}

export default App;