import Index from './home/Index';
import Profil from './home/Profil';
import './App.css';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';

function App() {
  return (
    <div className="App">

<nav>
    <ul>
        <li><a href="/">Home</a></li>
        <li><a href="/home">Profil</a></li>
    </ul>
</nav>

<Router>
<Routes>
  <Route path="/" element={<Index />} />
</Routes>
</Router>
</div>
  );
}



export default App;
