import { Navigate } from 'react-router-dom';
import { useAuth } from './AuthContext';

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
  const { isAuthenticated } = useAuth();
  console.log("🚀 ~ PrivateRoute ~ isAuthenticated:", isAuthenticated)
  return isAuthenticated ? children : <Navigate to="/login" />;
};

export default PrivateRoute;
