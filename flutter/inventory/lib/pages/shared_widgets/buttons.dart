import 'package:flutter/material.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';

class LoginButton extends StatelessWidget {
  const LoginButton({
    Key? key,
    required this.buttonPressed,
  }) : super(key: key);

  final VoidCallback buttonPressed;

  @override
  Widget build(BuildContext context) {
    return ElevatedButton(
      onPressed: buttonPressed,
      child: FittedBox(
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            FaIcon(
              FontAwesomeIcons.google,
              color: Colors.red[200],
            ),
            const SizedBox(width: 10),
            const Text('Sign In with Google'),
          ],
        ),
      ),
      style: ElevatedButton.styleFrom(
        shape: const StadiumBorder(),
        primary: Theme.of(context).primaryColor,
        onPrimary: Theme.of(context).primaryColorLight,
      ),
    );
  }
}

class LoginButtonAlt extends StatelessWidget {
  const LoginButtonAlt({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration:
          BoxDecoration(shape: BoxShape.circle, color: Colors.deepOrange[700]),
      child: Center(
        child: CircularProgressIndicator(
          color: Theme.of(context).primaryColorLight,
        ),
      ),
    );
  }
}

class IconTextRadio<T> extends StatelessWidget {
  const IconTextRadio({
    Key? key,
    required this.label,
    required this.value,
    required this.selectedValue,
    required this.icon,
    required this.onTap,
  }) : super(key: key);

  final String label;
  final T value;
  final T selectedValue;
  final IconData icon;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Card(
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(13),
        ),
        color: value == selectedValue
            ? Theme.of(context).highlightColor
            : Colors.grey[700],
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              FaIcon(icon),
              const SizedBox(height: 5),
              Text(
                label,
                style: TextStyle(color: Theme.of(context).primaryColorLight),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
