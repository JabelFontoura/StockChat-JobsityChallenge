import axios from "axios";
import { getToken } from "./Auth";

export const apiURL = "http://127.0.0.1:5000";

const api = axios.create({
  baseURL: apiURL + '/api'
});

api.interceptors.request.use(async config => {
  const token = getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;