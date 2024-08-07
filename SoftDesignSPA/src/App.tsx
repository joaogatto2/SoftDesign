import React from 'react';
import { Navigate, Route, BrowserRouter as Router, Routes } from 'react-router-dom';
import './App.css';
import Books from './pages/Books';
import Login from './pages/Login';
import BookDetail from './pages/BookDetail';
import PrivateRoute from './auth/PrivateRoute';
import { AuthProvider } from './auth/AuthContext';

const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/" element={<Navigate to="/books" />} />
          <Route path="/login" element={<Login />} />
          <Route path="/books" element={
            <PrivateRoute>
              <Books />
            </PrivateRoute>
            } />
          <Route path="/books/:id" element={
            <PrivateRoute>
              <BookDetail />
            </PrivateRoute>
            } />
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;