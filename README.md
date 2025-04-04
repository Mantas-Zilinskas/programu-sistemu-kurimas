## Struktūra

Projektas sudarytas iš dviejų dalių:

1. Frontend: React + Redux + Material-UI
2. Backend: .NET 8.0

## React Frontend Installation

### Įdiekite plėtinius priklausomybes
```bash
# Galite atlikti iš bet kurios direktorijos
npm install --save-dev webpack webpack-cli webpack-dev-server @babel/core @babel/preset-env @babel/preset-react babel-loader css-loader style-loader html-webpack-plugin
```

### Įdiekite programos priklausomybes
```bash
# Eikite į frontend direktoriją
cd frontend

# Įdiekite programos priklausomybes
npm install react react-dom @mui/material @emotion/react @emotion/styled react-redux @reduxjs/toolkit react-router-dom axios
```

## Klaidų diagnostika

Jeigu įdiegimo metu atsiranda klaidų:

1. Išvalykite npm talpyklą:
```bash
# Galite atlikti iš bet kurios direktorijos
npm cache clean --force
```

2. Ištrinkite node_modules ir package-lock.json:
```bash
# Eikite į frontend direktoriją
cd frontend

# Ištrinkite priklausomybių direktoriją
rm -rf node_modules package-lock.json
```

3. Naujai įdiekite priklausomybes:
```bash
# Eikite į frontend direktoriją
cd frontend

# Įdiekite priklausomybes
npm install
```

## Backend konfigūracija

Backend projektas naudoja .NET 8.0. Pirmiausia tikrinkite, ar yra įdiegtas .NET SDK:

```bash
# Galite atlikti iš bet kurios direktorijos
dotnet --version
```

## Problemos ir sprendimai

### Frontend klaidos

1. **Nepavyksta paleisti `npm start`**

   - Išvalykite npm talpyklą:
   ```bash
   # Galite atlikti iš bet kurios direktorijos
   npm cache clean --force
   ```

   - Ištrinkite `node_modules` ir `package-lock.json`:
   ```bash
   # Eikite į frontend direktoriją
cd frontend

# Ištrinkite priklausomybių direktoriją
rm -rf node_modules package-lock.json
   ```

   - Naujai įdiekite priklausomybes:
   ```bash
   # Eikite į frontend direktoriją
cd frontend

# Įdiekite priklausomybes
npm install
   ```

2. **Webpack build klaidos**

   - Patikrinkite, ar yra visos reikalingos dev priklausomybes
   - Patikrinkite, ar `webpack.config.js` yra tinkamai konfigūruotas
   - Paleiskite webpack su detaliu log'u:
   ```bash
   # Eikite į frontend direktoriją
cd frontend

# Paleiskite webpack su detaliu log'u
webpack --mode development --progress
   ```

## Papildoma informacija

- Frontend veikia ant porto 3000
- Backend veikia ant porto 5054
- Projektas naudoja Redux Toolkit valdyti būseną
- Frontend naudoja Material-UI komponentus
- Backend naudoja .NET 8.0