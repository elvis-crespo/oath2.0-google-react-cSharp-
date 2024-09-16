import axios from 'axios';
import './App.css'

function App() {
  const handleLogin = async () => {
    try {
      const response = await axios.get('https://localhost:7275/api/Auth/google-login');

      //url redirect
      window.location.href = response.data.url;  
    } catch (error) {
      console.error('Error when signing in with Google:', error);
    }
  };

  return (
    <>
       <div>
        <h1>Iniciar Sesión con Google</h1>
        <button onClick={handleLogin}>Iniciar Sesión</button>
      </div>
    </>
  )
}

export default App
