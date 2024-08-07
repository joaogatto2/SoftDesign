import { createContext, useContext, useState, ReactNode } from 'react';
import { AccountApi } from '../apis/account-api';
import {jwtDecode} from 'jwt-decode';

const accountApi = new AccountApi();

interface AuthContextType {
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<boolean>;
  logout: () => void;
  token: string | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
  const decodedToken = token ? jwtDecode(token) : null;
  const currentTime = Date.now() / 1000;

  if (decodedToken && decodedToken.exp && decodedToken.exp < currentTime) {
    setToken(null);
    localStorage.removeItem('token');
  }
  const isAuthenticated = !!token;

  const login = async (email: string, password: string) => {
    try {
      const response = await accountApi.login(email, password);
      if (response.data.success) {
        setToken(response.data.token);
        localStorage.setItem('token', response.data.token);
        return true;
      } else {
        alert(response.data.error);
        return false;
      }
    } catch (error) {
      console.error('Login failed', error);
      return false;
    }
  };

  const logout = () => {
    setToken(null);
    localStorage.removeItem('token');
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout, token }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
