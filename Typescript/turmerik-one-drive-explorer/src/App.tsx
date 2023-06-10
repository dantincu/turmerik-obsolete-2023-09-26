import { Routes, Route } from "react-router-dom";

import Layout from './pages/Layout';
import HomePage from "./pages/HomePage";
import ExplorerPage from "./pages/ExplorerPage";
import LoginPage from './pages/LoginPage';

import RequireAuth from './components/RequireAuth';

import AuthProvider from './components/AuthProvider';

const App = () => {
  return (
    <AuthProvider>
      <h1>Auth Example</h1>

      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route
            path="/protected"
            element={
              <RequireAuth>
                <ExplorerPage />
              </RequireAuth>
            }
          />
        </Route>
      </Routes>
    </AuthProvider>
  );
}

export default App;