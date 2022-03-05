import 'package:flutter/material.dart';

class ErrorPage extends StatelessWidget {
  const ErrorPage({
    Key? key,
    required this.exception,
    required this.stackTrace,
  }) : super(key: key);

  final Object exception;
  final StackTrace? stackTrace;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          children: [
            Text('Error: $exception'),
            Text(stackTrace!.toString()),
          ],
        ),
      ),
    );
  }
}
