import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Layout from './shared/Layout';

import ProfilePage from './home/Profile';
import SignUp from './home/SignUp';

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Layout />}>
          
          <Route path="profile" element={<ProfilePage />} />
          <Route path="signup" element={<SignUp />} />
        </Route>
      </Routes>
    </Router>
  );
};

export default App;
