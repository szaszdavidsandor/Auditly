const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;

export function validateEmail(value: string): string | null {
  const email = value.trim();
  if (!email) return 'Add meg az email címed.';
  if (!EMAIL_PATTERN.test(email)) return 'Érvénytelen email formátum (pl. nev@ceg.hu).';
  return null;
}

export function validatePassword(value: string): string | null {
  if (!value) return 'Add meg a jelszavad.';
  return null;
}
