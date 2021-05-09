export const TOKEN_KEY = "@stockChat-token";
export const USERID_KEY = "@stockChat-userid";
export const USERNAME_KEY = "@stockChat-username";
export const isAuthenticated = () => localStorage.getItem(TOKEN_KEY) !== null;
export const getToken = () => localStorage.getItem(TOKEN_KEY);
export const getUserId = () => localStorage.getItem(USERID_KEY);

export const login = data => {
  localStorage.setItem(TOKEN_KEY, data.accessToken);
  localStorage.setItem(USERID_KEY, data.id);
  localStorage.setItem(USERNAME_KEY, data.userName);
};

export const logout = () => {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(USERID_KEY);
  localStorage.removeItem(USERNAME_KEY);
};