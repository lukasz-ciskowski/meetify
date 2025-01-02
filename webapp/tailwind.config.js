/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './**/*.{razor,html,cshtml}',
    './wwwroot/**/*.js',
  ],
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/aspect-ratio'),
    require('@tailwindcss/typography'),
  ],
  "theme": {
    extend: {
      colors: {
        'primary': '#FF4057',
        'secondary': '#F75CFF',
        "background": "#282634",
      },
      fontFamily: {
        sans: ['Inter var', 'sans-serif'],
      },
    }
  },
}