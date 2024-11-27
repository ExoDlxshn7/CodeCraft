import React from 'react';
import { Outlet, Link } from 'react-router-dom';
import './Layout.css';

const Layout = () => {
  return (
    <div>
      <header>
        <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
          <div className="container">
            <Link className="navbar-brand" to="/">CodeCraft</Link>
            <div className="navbar-nav ml-auto">
              <Link className="nav-item nav-link" to="/">Home</Link>
              <Link className="nav-item nav-link" to="/profile">Profile</Link>
              <Link className="nav-item nav-link" to="/signup">Sign Up</Link>
            </div>
          </div>
        </nav>
      </header>

      <div className="container">
        <main role="main" className="pb-3">
          <Outlet /> {/* Dynamisk plass for innhold */}
        </main>
      </div>

      <footer className="text-center py-4 border-top">
        <p>&copy; 2024 CodeCraft. All Rights Reserved.</p>
      </footer>
    </div>
  );
};

export default Layout;
